import numpy as np
import rospy
from stable_baselines3 import SAC
from stable_baselines3.common.vec_env.dummy_vec_env import DummyVecEnv
from stable_baselines3.common.monitor import Monitor
import sys
import os
import rospy
import numpy as np
import torch
import PIL
from std_msgs.msg import String
from std_msgs.msg import Float32MultiArray
from RL_trainner_pkg.msg import TransitionMsg
from RL_trainner_pkg.srv import ProcessArray, ProcessArrayResponse
import gymnasium as gym
from gym import spaces
import concurrent.futures
# Define a custom Gym environment
class CustomEnv(gym.Env):
    def __init__(self):
        super(CustomEnv, self).__init__()
        self.observation_space = spaces.Box(low=-1.0, high=1.0, shape=(4,), dtype=np.float32)
        self.action_space = spaces.Box(low=-1.0, high=1.0, shape=(2,), dtype=np.float32)
        self.state = np.zeros(4)

    def reset(self):
        self.state = np.random.uniform(low=-1.0, high=1.0, size=(4,))
        return self.state

    def step(self, action):
        self.state = self.state + action  # Simplified dynamics
        reward = -np.sum(np.square(self.state))  # Reward function
        done = np.linalg.norm(self.state) > 2  # Episode ends if state is too large
        return self.state, reward, done, {}

    def render(self, mode='human'):
        pass

    def close(self):
        pass

class TrainnerNode:
    def __init__(self):
        self.state_dim = 4
        self.action_dim = 2
        self.trainTime = 10000
        self.timePassed = 0

        # Initialize components
        self.publishersDict = {}
        self.subscriberDict = {}
        self.serviceDict = {}
        self.callbackFuncToTopicName = {}
        self.handleFuncToServiceName = {}
        self.threadPoolExecutor = concurrent.futures.ThreadPoolExecutor(max_workers=20)

        # Initialize environment and agent
        self.env = DummyVecEnv([lambda: Monitor(CustomEnv())])
        self.agent = SAC('MlpPolicy', self.env, verbose=1)

    def run_node(self):
        self.run_function(self.updateNetwork, interval=0.2)
        self.run_function(self.updateInfo, interval=1)

        self.register_event_publisher(topic_name="/trainner_node/event/set_action", data_class=Float32MultiArray, queue_size=30)
        self.register_subscriber(topic_name="/unity/RL_Agent/transition", data_class=TransitionMsg, callback=self.onCall_subscribe_transition)
        self.register_service_server(service_name="/trainner_node/service/sample_action", service_class=ProcessArray, handle_func=self.onCall_handleService_sampleAction)

    def run_function(self, func, interval: float):
        rospy.Timer(rospy.Duration(interval), func)

    def updateNetwork(self, event):
        if self.timePassed < self.trainTime:
            self.agent.learn(total_timesteps=1)
            self.timePassed += 1
    def updateInfo(self,event):
        self.timePassed+=1
        print("[INFO][updateInfo]timePassed={0}".format(self.timePassed))
        print("[INFO][updateInfo]self.agent.memory.size={0}".format(self.agent.memory.size()))
        print("[INFO][updateInfo]num_NetowrkUpdate:{0}".format(self.num_update))
        
    # ------------------------------------Publishers-----------------------------
    def register_event_publisher(self, topic_name: str, data_class, queue_size=10):
        publisher = rospy.Publisher(name=topic_name, data_class=data_class, queue_size=queue_size)
        if topic_name in self.publishersDict:
            print("[ERROR][LMM_Sf_Node]register publisher with name:{0} twice!!".format(topic_name))
        else:
            self.publishersDict[topic_name] = publisher

    def register_run_publisher(self, topic_name: str, data_class, call_publish_func, duration: int):
        publisher = rospy.Publisher(name=topic_name, data_class=data_class, queue_size=10)
        # store topic name and publisher in a dictionary
        if topic_name in self.publishersDict:
            print("[ERROR][LMM_Sf_Node]register publisher with name:{0} twice!!".format(topic_name))
        else:
            self.publishersDict[topic_name] = publisher
        # store callback function and topic name in a dictionary
        if call_publish_func in self.callbackFuncToTopicName:
            print("[ERROR][LMM_Sf_Node]register callback func with name:{0} twice!!".format(call_publish_func))
        else:
            self.callbackFuncToTopicName[call_publish_func] = topic_name
        rospy.Timer(rospy.Duration(duration), call_publish_func)
    # ---------------------------------------Subscribers---------------------------------------------------------
    def register_subscriber(self, topic_name: str, data_class, callback):
        subscriber = rospy.Subscriber(name=topic_name, data_class=data_class, callback=callback)
        # store topic name and subscriber in a dictionary
        if topic_name in self.subscriberDict:
            print("[ERROR][LMM_Sf_Node]register subscriber with name:{0}".format(topic_name))
        else:
            self.subscriberDict[topic_name] = subscriber
        # store callback function and topic name in a dictionary
        if callback in self.callbackFuncToTopicName:
            print("[ERROR][LMM_Sf_Node]register callback func with name:{0} twice!!".format(callback))
        else:
            self.callbackFuncToTopicName[callback] = topic_name
    def onCall_subscribe_transition(self,msg):
        topic_name = self.callbackFuncToTopicName[self.onCall_subscribe_transition]
        # print("[INFO][onCall_subscribe_transition]state:{0};action:{1};reward:{2};next_state:{3}".format(msg.state,msg.action,msg.reward,msg.next_state))
        state = np.array(msg.state)
        action = np.array(msg.action)
        reward = np.array(msg.reward)
        next_state = np.array(msg.next_state)
        trancated_flag = msg.trancated_flag
        transition = (state,action,reward,next_state)
        if(trancated_flag==False):
            self.agent.replay_buffer.add(state, action, reward, next_state, trancated_flag)

    # -------------------------------------Service----------------------------------------------------------
    def register_service_server(self,service_name:str,service_class,handle_func):
        service = rospy.Service(service_name, service_class, handle_func)
    def onCall_handleService_sampleAction(self, req):
        future = self.threadPoolExecutor.submit(self._thread_processRequest_sampleAction, req)
        response_data = future.result()
        response = ProcessArrayResponse()
        response.output = Float32MultiArray(data=response_data.data)
        return response

    def _thread_processRequest_sampleAction(self, req):
        state = np.array(req.input.data)
        if self.timePassed < self.trainTime:
            action, _states = self.agent.predict(state, deterministic=False)
        else:
            action, _states = self.agent.predict(state, deterministic=True)
        response = Float32MultiArray(data=action.tolist())
        return response
    def train_agent(self):
        self.agent.learn(total_timesteps=self.trainTime)






if __name__ == "__main__":
    # -----------------------Main-------------------
    rospy.init_node(name="trainner_node", anonymous=True, log_level=rospy.INFO)
    try:
        node = TrainnerNode()
        node.run_node()
        rospy.spin()

    except rospy.ROSInterruptException:
        pass
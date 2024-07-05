
import sys
import os
import rospy
import numpy as np
import torch
import PIL
from std_msgs.msg import String
from std_msgs.msg import Float32MultiArray
from RL_trainner_pkg.msg import TransitionMsg
from SAC import SAC_Agent
class TrainnerNode:
    def __init__(self) -> None:
        #Parameters
        self.state_dim = 2
        self.action_dim = 2
        # initialize components
        self.publishersDict = {}
        self.subscriberDict = {}
        self.callbackFuncToTopicName = {}
        #Storage Field
        self.cur_state:np.ndarray=None
        
        self.agent = SAC_Agent(self.state_dim,self.action_dim)
    def run_node(self):
        #1.Register Function Runner
        self.run_function(self.updateAction,interval=0.5)
        #2. Register Publishers
        self.register_event_publisher(topic_name="/trainner_node/event/set_action",data_class=Float32MultiArray,queue_size=30)
        #3.Register Subscribers
        self.register_subscriber(topic_name="/unity/RL_Agent/transition",data_class=TransitionMsg,callback=self.onCall_subscribe_transition)
        

    #----------------------------------FunctionRunner---------------------------
    def run_function(self, func, interval: float):
        rospy.Timer(rospy.Duration(interval), func)
    def updateAction(self,event):
        if(self.cur_state is not None):
            action_publisher = self.publishersDict["/trainner_node/event/set_action"]
            action = self.agent.get_action(self.cur_state,train=True)
            # Create a Float32MultiArray message
            action_msg = Float32MultiArray()
            
            # Populate the data field with the action values
            action_msg.data = action.tolist()  # Ensure action is converted to a list if it's a numpy array
            
            # Publish the message
            action_publisher.publish(action_msg)


        
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

        transition = (state,action,reward,next_state)
        self.agent.memory.put(transition)
        self.cur_state = state
if __name__ == "__main__":
    # -----------------------Main-------------------
    rospy.init_node(name="trainner_node", anonymous=True, log_level=rospy.INFO)
    try:
        node = TrainnerNode()
        node.run_node()
        rospy.spin()

    except rospy.ROSInterruptException:
        pass
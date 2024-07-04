
import sys
import os
import rospy
import numpy as np
import torch
import PIL
from std_msgs.msg import String
from std_msgs.msg import Float32MultiArray
class TrainnerNode:
    def __init__(self) -> None:
        # initialize components
        self.publishersDict = {}
        self.subscriberDict = {}
        self.callbackFuncToTopicName = {}
        pass
    def run_node(self):
        #2.
        self.register_subscriber(topic_name="/unity/RL_Agent/observationsList",data_class=Float32MultiArray,callback=self.onCall_subscribe_observation)
        #3. 

    #----------------------------------FunctionRunner---------------------------
    def run_function(self, func, interval: int):
        rospy.Timer(rospy.Duration(interval), func)
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
    def onCall_subscribe_observation(self,msg):
        topic_name = self.callbackFuncToTopicName[self.onCall_subscribe_observation]
        print("observation:{0}".format(msg.data))
        pass
if __name__ == "__main__":
    # -----------------------Main-------------------
    rospy.init_node(name="lmm_sf_node", anonymous=True, log_level=rospy.INFO)
    try:
        node = TrainnerNode()
        node.run_node()
        rospy.spin()

    except rospy.ROSInterruptException:
        pass
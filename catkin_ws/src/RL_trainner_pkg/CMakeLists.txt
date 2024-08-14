cmake_minimum_required(VERSION 3.0.2)
project(RL_trainner_pkg)

find_package(catkin REQUIRED COMPONENTS
  roscpp
  rospy
  std_msgs
  message_generation
)

add_message_files(
  FILES
  TransitionMsg.msg
  Float32IDMsg.msg
)
add_service_files(
  FILES
  ProcessArray.srv
)
generate_messages(
  DEPENDENCIES
  std_msgs
)

catkin_package(
  CATKIN_DEPENDS message_runtime std_msgs
)

include_directories(
  ${catkin_INCLUDE_DIRS}
)

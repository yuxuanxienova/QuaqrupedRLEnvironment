import torch
from torch.utils.tensorboard import SummaryWriter
import numpy as np

# Define a function to perform the test
def test_tensorboard_logging(log_dir):
    # Create a SummaryWriter
    writer = SummaryWriter(log_dir)

    # Log some scalar values
    for step in range(100):
        writer.add_scalar('test/scalar1', np.sin(step / 10), step)
        writer.add_scalar('test/scalar2', np.cos(step / 10), step)

    # Flush and close the writer
    writer.flush()
    writer.close()
    print(f"Logs have been written to {log_dir}")

# Specify the log directory
log_dir = "./catkin_ws/src/RL_trainner_pkg/logs"

# Run the test
test_tensorboard_logging(log_dir)

# Print instructions to run TensorBoard
print("Run the following command to start TensorBoard and view the logs:")
print(f"tensorboard --logdir={log_dir}")
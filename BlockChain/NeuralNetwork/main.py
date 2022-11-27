import torch
import torch.nn as nn
import torch.nn.functional as F
import pandas as pd
import matplotlib.pyplot as plt


class Net(nn.Module):

    def __init__(self):
        super(Net, self).__init__()
        self.fc1 = nn.Linear(2, 2)
        self.fc2 = nn.Linear(2, 3)
        self.fc3 = nn.Linear(3, 1)

    def forward(self, x):
        x = F.sigmoid(self.fc1(x))
        x = F.sigmoid(self.fc2(x))
        x = F.sigmoid(self.fc3(x))
        return x


net = Net().double()
print(net)

df = pd.read_csv('test_data_100.csv', sep=";")

input = torch.tensor(df[['x1', 'x2']].values)
target = torch.tensor(df[['y']].values)

loss_function = nn.MSELoss()
optimizer = torch.optim.SGD(net.parameters(), lr=0.01)
losses = []
for epoch in range(100):
    pred_y = net(input)
    loss = loss_function(pred_y, target)
    losses.append(loss.item())

    net.zero_grad()
    loss.backward()

    optimizer.step()

plt.plot(losses)
plt.ylabel('loss')
plt.xlabel('epoch')
plt.title("Learning rate %f"%(0.01))
plt.show()

print(net.fc1.weight)
print(net.fc2.weight)
print(net.fc3.weight)

print(losses[len(losses)-1])


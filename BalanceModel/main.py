import random

import pandas as pd
import numpy as np

df = pd.read_csv('data.csv', sep=';')
X = df.copy().drop(columns=['-', 'Y'], axis=1)
Y = df[['Y']].copy()

data_x = X.to_numpy()
data_y = Y.to_numpy()

x_i = []

for i in range(len(data_x)):
    i_sum = sum(data_x[i], data_y[i])[0]
    x_i.append(i_sum)

A = []
for i in range(len(data_x)):
    A.append([])
    for j in range(len(data_x[i])):
        A[i].append(data_x[i][j]/x_i[i])

E_minus_A = np.subtract(np.eye(len(A)), A)

E_minus_A_inv = np.linalg.inv(E_minus_A)

print('X:')
print(X)
print('Y:')
print(Y)
print('x_i:')
print(x_i)

new_Y = []
for i in range(len(data_y)):
    new_Y.append(random.randint(1000, 1000000))

new_X = np.multiply(E_minus_A_inv, new_Y)


print('A:')
print(A)

print('E-A:')
print(E_minus_A)

print("E_minus_A_inv:")
print(E_minus_A_inv)

print('new_Y:')
print(new_Y)

print('new_X:')
print(new_X)


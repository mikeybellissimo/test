# Created by Youssef Elashry to allow two-way communication between Python3 and Unity to send and receive strings

# Feel free to use this in your individual or commercial projects BUT make sure to reference me as: Two-way communication between Python 3 and Unity (C#) - Y. T. Elashry
# It would be appreciated if you send me how you have used this in your projects (e.g. Machine Learning) at youssef.elashry@gmail.com

# Use at your own risk
# Use under the Apache License 2.0

# Example of a Python UDP server

import UdpComms as U
import time
import pandas as pd

# Create UDP socket to use for sending (and receiving)
sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)


while True:


    taxonomy = pd.DataFrame({'children' : ['human', 'primate', 'mammal'], 'parents' : ['primate', 'mammal', 'animal']})
    relations = pd.DataFrame({'entity' : ['human', 'primate', 'mammal'], 'relationship' : ['hasSize', 'hasBodyPart', 'birthType'], 'distribution' : [0, 1, 2]})
    distributions = pd.DataFrame({'id' : [0,1,2], 'type' : ['normal', 'discrete', 'discrete'], 'mean' : [.1778, 0, 0], 'variance' : [.01016, 0 , 0 ], 'discrete_id' : ['0', 'a1', 'a2']})
    discrete_probabilities = pd.DataFrame({'discrete_id' : ['a1', 'a1', 'a2', 'a2'], 
    'value' : ['Opposable Thumbs', 'Non-Opposable Thumbs', 'Oviparous', 'Viviparous'], 
    'value_type' : ['class', 'class', 'class', 'class'], 
    'probabilitiies' : [0.99, 0.01, 0.999, 0.001]})
    

    # combine all the different knowledge sources into one json string to pass through to unity 
    buffered_json = '{"taxonomy" : ' + taxonomy.to_json() +  ', "relations" : ' + relations.to_json() + ', "distributions" : ' + distributions.to_json() + ', "discrete_probabilities" : ' + discrete_probabilities.to_json() + "}" 
    print(buffered_json)
    sock.SendData(buffered_json) # Send this string to other application
    
    data = sock.ReadReceivedData() # read data

    if data != None: # if NEW data has been received since last ReadReceivedData function call
        print(data) # print new received data

    time.sleep(1)
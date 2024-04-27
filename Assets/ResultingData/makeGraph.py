import re
import matplotlib.pyplot as plt

def parse_file(filename):
    res = []
    d = []
    i = 1
    with open(filename, 'r') as file:
        for line in file:
            num = int(line.split()[-1])
            res.append(num)
            d.append(i)
            i += 1
    return res, d

def create_graph(res,d):
    plt.plot(res, marker='o', linestyle='dotted')
    plt.yscale('linear')
    #plt.yticks(range(int(min(res)), int(max(res))+1, 2), labels = res)
    

    plt.title('Result vs Location')
    plt.xlabel('Location as iteration')
    plt.ylabel('Num of Track Units Seen')
    plt.grid(False)
    plt.show()

if __name__ == "__main__":
    filename = 'Assets/ResultingData/AnticipationNumber.txt'  # Replace 'data.txt' with the name of your file
    x_values,d = parse_file(filename)
    create_graph(x_values,d)

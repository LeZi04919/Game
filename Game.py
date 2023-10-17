import random
import os

hashMap = { 0:"剪刀",1:"石头",2:"布" }
def GetRandomNum():
    randomNum = random.random()
    return randomNum

def GetMachineHand(RandomNum):
    if float(RandomNum) < 1/3:
        return 0 #剪刀
    elif float(RandomNum) < 2/3:
        return 1 #石头
    else:
        return 2 #布  
def GetResult(userInput,machineHand):
    print("你的出拳:",hashMap[userInput])
    print("电脑出拳:",hashMap[machineHand])

def SetVictory(userInput,machineHand):
    GetResult(userInput,machineHand)
    print("\n你赢了!")
    os.system('pause')

def SetNegative(userInput,machineHand):
    GetResult(userInput,machineHand)
    print("\n你输了!")
    os.system('pause')


def main():
    numList = ["0","1","2","3","4","5","6","7","8","9"]
    victoryCount = 0
    negativeCount = 0
    drawCount = 0
    while True:
        totalCount = victoryCount + negativeCount
        print("=========================================")
        print("游戏规则:")
        print("\"0\"代表剪刀")
        print("\"1\"代表石头")
        print("\"2\"代表布")
        print("=========================================")
        print("总场数:",totalCount)
        print("胜场数:",victoryCount)
        print("负场数:",negativeCount)
        if totalCount !=0 :
            print("胜率:",victoryCount / totalCount *100,"%")
        else:
            print("胜率:Unknow")
        print("\n到你出拳了！")
        userInput = input()
        if (userInput not in numList) or (userInput not in ["0","1","2"]):
            print("?\n无效输入，将自动判负")
            os.system('pause')
            negativeCount += 1
            continue
        else:
            userInput = int(userInput)
            machineHand = GetMachineHand(GetRandomNum())
            if userInput == machineHand:
                drawCount += 1
                GetResult(userInput,machineHand)
                print("本局平局\n")
                os.system('pause')
            elif userInput == 0:
                if machineHand == 1:
                    negativeCount += 1
                    SetNegative(userInput,machineHand)
                else:
                    victoryCount += 1
                    SetVictory(userInput,machineHand)
            elif userInput == 1:
                if machineHand == 2:
                    negativeCount += 1
                    SetNegative(userInput,machineHand)
                else:
                    victoryCount += 1
                    SetVictory(userInput,machineHand)
            else:
                if machineHand == 0:
                    negativeCount += 1
                    SetNegative(userInput,machineHand)
                else:
                    victoryCount += 1
                    SetVictory(userInput,machineHand)                 
        os.system("cls")

if __name__ == "__main__":
    main()
    



import os
import GameClass

game_over = False
Area:str #当前区域
Player:GameClass.Player = GameClass.Player()
TotalStepCount:int #总步数
AreaStep:int #完成当前区域所需步数


def Combat(monsters:list[GameClass.Monster]) -> None:
    os.system('cls')
    print("#############################################################")
    print("                      你陷入了一场战斗                        \n")
    print("           目标:")
    for i in monsters:
        print("                [",monsters.index(i),"]",i.Name,f"({i.Health}/{i.MaxHealth})")
    print("#############################################################")
    print("              1.攻击")
    print("              2.使用技能")
    print("              3.物品")
    print("              4.逃跑")
    userInput = input("现在，请选择:")
    if len(userInput) > 1 or len(userInput) == 0:
        print("无效输入，请重新输入")
        os.system('pause')
        Combat(monsters)
    
    


if __name__ == "__init__":
    Area = "新手村"
    TotalStepCount = 0
    AreaStep = 100
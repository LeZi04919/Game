import os
from xmlrpc.client import Boolean
import GameClass

game_over = False
Area:str #当前区域
Player:GameClass.Player = GameClass.Player()
TotalStepCount:int #总步数
AreaStep:int #完成当前区域所需步数

def Attack(targets:list[GameClass.Monster]) -> None: #攻击处理
    for target in targets:
        damage = Player.Attacked(target)
        if damage > 0:
            print(f"你对{target.Name} {targets.index(target)}造成了共计{damage}点伤害")
        else:
            print(f"你的攻击被{target.Name} {targets.index(target)}闪避了,造成了0点伤害")

def SelectTarget(skill:GameClass.Skill,monsters:list[GameClass.Monster],indexRange:list[str] = ["Monster"]) -> None: #选择目标
    os.system('cls')
    print("#############################################################")
    print("                          请选择目标")
    if {"Self","Player"} & set(indexRange):
        print("[S] Player")
    for monster in monsters:
        print(f"[{monsters.index(monster)}] {monster.Name} ({monster.Health}/{monster.MaxHealth})")
    userInput = input("请输入序号:")
    if "S" in userInput:
        Player.ReleaseSkill(skill,Player)
    userInput = int(userInput)
    if userInput > len(monsters) - 1:
        input("无效输入，请重新输入")
        SelectTarget(skill,monsters,indexRange)
    Player.ReleaseSkill(skill,monsters[userInput])
    input(f"你对[{userInput}] {monsters[userInput].Name} 使用了技能 {skill.Name} !")
    Combat(monsters)
        
def UseSkill(skill:GameClass.Skill,monsters:list[GameClass.Monster]):
    if {"Self","Player"} & set(skill.EffectiveObject):
        Player.ReleaseSkill(skill,Player)
    else:
        SelectTarget(skill,monsters,skill.EffectiveObject)

def SkillDetails(skill:GameClass.Skill,monsters:list[GameClass.Monster]) -> None: #查看技能详情
    os.system('cls')
    coolDownList = Player.inCoolDown
    print("#############################################################")
    print(f"                       {skill.Name}")
    print(f"技能效果:{skill.Effect}")
    print(f"作用对象:{skill.EffectiveObject}")
    print(f"生效轮数:{skill.EffectiveRounds}")
    print("#############################################################")
    if skill not in coolDownList.keys():
        userInput = input("1.使用该技能\n2.返回")
        if "2" in userInput:
            ViewSkills(monsters)
        if "1" in userInput:
            if "AreaAttack" in skill.Effect:
                Combat(monsters,int(skill.Value))
            else:
                UseSkill(skill,monsters)
    else:
        input()

def ViewSkills(monsters:list[GameClass.Monster]) -> bool: #查看技能
    os.system('cls')
    skills = Player.Skills
    coolDownList = Player.inCoolDown
    print("#############################################################")
    print("                           技能                              \n")
    for skill in skills:
        if skill in coolDownList.keys():
            print(f"[{skills.index(skill)}] {skill.Name} (CD剩余回合:{coolDownList[skill]})")
        else:
            print(f"[{skills.index(skill)}] {skill.Name}")
    print("[Q] 退出")
    print("\n#############################################################")
    userInput = input("请输入序号:")
    if "Q" in userInput:
        return False
    userInput = int(userInput)
    SkillDetails(skills[userInput],monsters)
    return True

def Combat(monsters:list[GameClass.Monster],index:int = 1) -> None: # 战斗UI
    os.system('cls')
    print("#############################################################")
    print("                      你陷入了一场战斗                        ")
    print("           目标:")
    for i in monsters:
        print("                [",monsters.index(i),"]",i.Name,f"({i.Health}/{i.MaxHealth})")
    print("#############################################################")
    if index > 1:
        userInput:str = input(f"(最大目标数:{index})\n以\",\"作为分隔符\n示例:\"0,1,2\"\n请输入目标:")
        targets:list[GameClass.Monster] = []
        for i in userInput.split(","):
            i = int(i)
            if i > len(monsters) - 1:
                break
            targets.append(monsters[i])
        if len(targets) != index:
            input("存在无效目标，请重新输入")
            Combat(monsters,index)
        else:
            Attack(targets)
    else:
        print("              1.攻击")
        print("              2.使用技能")
        print("              3.物品")
        print("              4.逃跑")
        userInput = input("现在，请选择:")
        if len(userInput) > 1 or len(userInput) == 0:
            print("无效输入，请重新输入")
            os.system('pause')
            Combat(monsters)
            
        if "1" in userInput:
            monsterIndex = int(input("请输入敌人序号\"[]内的数字\":"))
            if monsterIndex > len(monsters) - 1: #序号不得大于列表长度
                input("存在无效目标，请重新输入")
                Combat(monsters,index)
            else:
                Attack([monsters[monsterIndex]])
        elif "2" in userInput:
            ViewSkills(monsters)
    
if __name__ == "__init__":
    Area = "新手村"
    TotalStepCount = 0
    AreaStep = 100
import random

game_over = False
#Monster实体
class Monster:
    Name = ""  #怪物名称
    Health = 0 #生命值
    Attack = 0 #攻击力
    Armor = 0 #防御力
    isBoss = False

    def __init__(self,Name,Health,Armor,Attack,isBoss):
        self.Name = Name
        self.Health = Health
        self.Armor = Armor
        self.Attack = Attack
        self.isBoss = isBoss
    def __init__(self,Name,Health,Armor,Attack):
        self.__init__(self,Name,Health,Armor,Attack,False)

class PlayerClass:
    Health = 20
    Attack = 5
    Armor = 1
    Experience = 0
    Level = 1
    Buff = {} # Buff/DeBuff名 : Buff层数
    Item = []

    def __init__(self):
        pass
# Player初始        
Player = PlayerClass()

# 定义怪物列表
monsters = [
    Monster("史莱姆",10,1,1),
    Monster("红史莱姆",10,1,1),
    Monster("蓝史莱姆",10,1,1),
    Monster("哥布林",15,1,2),
    Monster("兽人",20,2,4), 
    Monster("史莱姆王",150,10,10,True),
    Monster("龙",500,10,50,True)]

# 定义玩家攻击和生命提升的概率
attack_probability = 0.5
life_increase_probability = 0.2
treasure = "我的爱"

while not game_over:
    # 随机选择一个怪物
    monsters = random.choice(monsters)

    # 如果怪物是BOSS，则增加难度
    if monsters == BOSS["名称"]:
        monsters["防御"] += 2

    # 生成攻击和生命提升的概率
    attack_probability = random.uniform(0.1, 0.5)
    life_increase_probability = random.uniform(0.05, 0.2)

    # 生成攻击
    attack = input("您想要攻击 " + monsters + " 吗?(y/n)")

    # 如果选择攻击，则进行攻击
    if attack == "y":
        damage = random.randint(1, 5)
        monsters["health"] -= damage

        # 如果怪物死亡，则游戏结束
        if monsters["health"] <= 0:
            game_over = True
            print("您已经击败了 " + monsters + "！您找到了宝物： " + treasure + "。")
        else:
            print("您已经对 " + monsters + " 造成了 " + str(damage) + " 点伤害。")

    # 如果选择生命提升，则提升生命值
    if attack == "n":
        life_increase = input("您想要提升生命值吗?(y/n)")

        # 如果选择生命提升，则提升生命值
        if life_increase == "y":
            life += 1
            print("您的生命值已经增加 1。")
        else:
            print("您选择了不提升生命值。")

# 如果玩家胜利，则输出胜利信息
if not game_over:
    print("您已经失去了所有生命。游戏结束。")
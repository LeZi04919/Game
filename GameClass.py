import math
import random
from typing import Self
import Game
    
class Item:
    Name:str = ""
    Stackable:bool = True
    Count:int = 0
    MaxStackCount:int = 0
    Level:int = 1
        
    def __init__(self,Name:str,Stackable:bool,Count:int,MaxStackCount:int):
        self.Name = Name
        self.Stackable = Stackable
        self.Count = Count
        self.MaxStackCount = MaxStackCount
        
    def Use(self,UsageCount:int) -> bool:
        if UsageCount > self.Count:
            raise Exception(f"使用数量\"{UsageCount}\"超出了物品\"{self.Name}\"的最大数量，该物品所持数量为:{self.Count}")
        else:
            self.Count -= UsageCount
            if self.Count == 0:
                return False
            else:
                return True
    
    def Stack(self,OtherItem):
        if OtherItem.Stackable and self.Stackable:
            raise Exception(f"物品\"{self.Name}\"与物品\"{OtherItem.Name}\"中有一个或多个不可堆叠")
        if OtherItem.Name == self.Name:
            raise Exception(f"物品\"{self.Name}\"与物品\"{OtherItem.Name}\"不属于同一类")
        canStackCount = self.MaxStackCount - self.Count
        if OtherItem.Count < canStackCount:
            self.Count += OtherItem.Count
            return True
        else:
            self.Count += canStackCount
            OtherItem.Count -= canStackCount
            return OtherItem
  
class Skill:
    Name = ""
    Effect = ""
    EffectiveRounds = 0 #生效轮数
    EffectiveObject = []
    Value = 0
    CoolDown = 0 #单位为轮
    
    def __init__(self,Name:str,Effect:str,EffectiveRounds:int,EffectiveObject:list[str],Value:float,CoolDown:int):
        self.Name = Name
        self.Effect = Effect
        self.EffectiveRounds = EffectiveRounds
        self.EffectiveObject = EffectiveObject
        self.Value = Value
        self.CoolDown = CoolDown     
class Prefab:
    MaxHealth:float
    Health:float
    Attack:float
    Armor:float
    Experience:float #经验值
    ExpMaxLimit:float
    Level:int = 1
    Dodge:float = 0 # 闪避率
    Buff:dict[str,int] = {} # Buff/DeBuff名 : Buff层数
    BuffValue:dict[str,float] = {} # Buff名 : Buff效果
    Skills:list[Skill] = []
    inCoolDown:list[Skill] = []
    Equipment:list = []
    
    def __init__(self,MaxHealth,Health,Attack,Armor,Experience,ExpMaxLimit,Level,Dodge,Skills):
        self.MaxHealth = MaxHealth
        self.Health = Health
        self.Attack = Attack
        self.Armor = Armor
        self.Experience = Experience
        self.ExpMaxLimit = ExpMaxLimit
        self.Level = Level
        self.Dodge = Dodge
        self.Skills = Skills
        self.Upgrade(Level)
        
    def Upgrade(self,Level:int): #升级
        self.Level = Level
        self.Experience -= self.ExpMaxLimit
        self.ExpMaxLimit += 5 * (self.Level - 1) 
        self.Health *=  math.pow(1.057,Level - 1) 
        self.Attack *=  math.pow(1.062,Level - 1)
    def Injuried(self,Attack:float) -> float: #被攻击
        objectAttributes = self.ApplyBuff()
        randomNum = random.random()
        Dodge = self.Dodge + objectAttributes["Dodge"]
        Armor = objectAttributes["Armor"]
        if randomNum < Dodge:
            _Attack = min(Attack - Armor,Attack * 0.1)
            self.Health -= _Attack
            if self.Health <= 0 :
                return -1
            return _Attack
        else:
            return 0
     
    def ApplyBuff(self) ->dict[str,float]:
        Attack:float = 0
        Armor:float = 0
        Dodge:float = 0
        for buff in self.Buff.keys():
            if buff == "AttackUp" or buff == "AttackDown":
                Attack = self.Attack * self.BuffValue[buff]
            elif buff == "ArmorUp" or buff == "ArmorDown":
                Armor = self.Armor * self.BuffValue[buff]    
            elif buff == "DodgeUp":
                Dodge = min(self.Dodge + self.BuffValue[buff],1)
            elif buff == "DodgeDown":
                Dodge = max(self.Dodge - self.BuffValue[buff],0)
        for equip in self.Equipment:
            if "Weapon" in type(equip): #剑
                Attack += equip.Attack
            elif "WearableArmor" in type(equip): #护甲
                Armor += equip.ArmorValue
                Dodge += equip.ArmorDodge
        return {"Attack":Attack,"Armor":Armor,"Dodge":Dodge}
    
    def ReleaseSkill(self,skill:Skill,object:Self) -> bool: #释放技能
        
        if skill in self.inCoolDown:
            return False
        else:
            if skill.Effect in object.Buff.keys():
                object.Buff[skill.Effect] += skill.EffectiveRounds
                if "Up" in skill.Effect:
                    object.BuffValue[skill.Effect] = max(object.BuffValue[skill.Effect],skill.Value)
                elif "Down" in skill.Effect:
                    object.BuffValue[skill.Effect] = min(object.BuffValue[skill.Effect],skill.Value)
            else:
                object.Buff[skill.Effect] = skill.EffectiveRounds
                object.BuffValue[skill.Effect] = skill.Value
            return True
      
    def Attacked(self,object:Self): #攻击
        objectAttributes = self.ApplyBuff()
        object.Injuried(objectAttributes["Attack"])          

class Monster(Prefab):
    Name = ""  #怪物名称
    isBoss = False

    def __init__(self,Name:str,MaxHealth:float,Armor:float,Attack:float,Dodge:float,isBoss:bool,Skills:list[Skill],Level:int = 1,Equipment:list = []):
        self.Name = Name
        self.MaxHealth = MaxHealth
        self.Armor = Armor
        self.Attack = Attack
        self.Dodge = Dodge
        self.isBoss = isBoss
        self.Skills = Skills
        self.Level = Level
        self.Equipment = Equipment
        super().__init__(MaxHealth,MaxHealth,Attack,Armor,0,1,Level,Dodge,Skills)
        
class Player(Prefab):
    MaxHealth = 20
    Experience = 0 #经验值
    ExpMaxLimit = 25
    Level = 1
    Dodge:float = 0 # 闪避率
    Items:list[Item] = []
    Skills:list[Skill] = [ 
        Skill("强力击","AttackUp",1,["Player"],2.0,3),
        Skill("广域攻击","AreaAttack",1,["Monster"],3,2)        
    ]
    

    def __init__(self):
        super().__init__(
            20,
            20,
            5,
            1,
            0,
            25,
            1,
            0,
            [
                Skill
                (
                    "强力击",
                    "AttackUp",
                    1,
                    ["Player"],
                    2.0,
                    3
                )        
    ])
    
    def DisposeItem(self,item:Item):
        self.Items.remove(item)
        
    def AddExp(self,Exp:float): #经验值变动
        self.Experience += Exp
        if(self.Experience >= self.ExpMaxLimit):
            self.Upgrade(self.Level + 1)
    
    def Upgrade(self,Level:int): #升级
        self.Level = Level
        self.Experience -= self.ExpMaxLimit
        self.ExpMaxLimit += 5 * (self.Level - 1) 
        self.Health += 20 * math.pow(1.055,Level - 1) - self.MaxHealth
        self.MaxHealth = 20 * math.pow(1.055,Level - 1) 
        self.Attack = 5 * math.pow(1.06,Level - 1) 
        print(f"你升级了!\n距离下一级还需要{self.ExpMaxLimit - self.Experience} EXP")
        self.AddExp(0)
                       
class Weapon(Item):
    
    Attack:float = 0
    Durability:int = 20 #耐久度，单位为轮    
    MaxDurability:int = 20
    
    def __init__(self,Name:str,Attack:float,MaxDurability:int,Level:int = 1):
        self.Name = Name
        self.Stackable = False
        self.Level = Level
        self.Attack = Attack + int(random.uniform(-(Attack * 0.2),(Attack * 0.2)))
        if random.random() <= 0.8:
            self.Durability = MaxDurability
            self.MaxDurability = MaxDurability
        else:
            self.MaxDurability = MaxDurability
            self.Durability = int(random.uniform(-MaxDurability * 0.5,MaxDurability * 0.95))
        super().__init__(Name,False,1,1)
        
    def NextRound(self) -> bool:
        self.Durability -= 1
        if self.Durability <= 0:
            Game.Player.DisposeItem(self)
            return True
        else:
            return False

        
class WearableArmor(Item):
    
    ArmorValue:int = 0
    ArmorDodge:float = 0
    
    def __init__(self, Name:str,ArmorValue:int,ArmorDodge:float,Level:int = 1):
        self.ArmorValue = ArmorValue + int(random.uniform(-(ArmorValue * 0.2),(ArmorValue * 0.2)))
        self.ArmorDodge = ArmorDodge + random.uniform(-0.1,0.1)
        self.Level = Level
        super().__init__(Name, False, 1, 1)
  
class SpecialItem(Item):
    
    Effect:str = ""
    EffectiveRounds:int = 0 #生效轮数
    EffectiveObjects:list = []
    Value:float = 0
    
    def __init__(self, Name:str, Count:int, MaxStackCount:int,Effect:str,EffectiveRounds:int,EffectiveObject:list,Value:float,Level:int = 1):
        super().__init__(Name, True, Count, MaxStackCount)
        self.Effect = Effect
        self.EffectiveRounds = EffectiveRounds + int(random.uniform(-EffectiveRounds * 0.5,EffectiveRounds * 0.5))
        self.EffectiveObject = EffectiveObject
        self.Value = Value + random.uniform(-Value * 0.5,Value * 0.5)
        self.Level = Level
        
    def Use(self,Object) -> bool:
        canUse = False
        for i in self.EffectiveObjects:
            if type(Object) in type(i) :
                canUse = True
        if not canUse:
            return False
        else:
            if self.Effect in Object.Buff.keys():
                Object.Buff[self.Effect] += self.EffectiveRounds
            else:
                Object.Buff[self.Effect] = self.EffectiveRounds
            return True
            
        

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using RoguelikeGame;
using RoguelikeGame.Interfaces;
using static RoguelikeGame.Game;

namespace RoguelikeGame.Class
{
    internal class Skill : IReleasable
    {
        /// <summary>
        /// 该技能的名称
        /// </summary>
        public required string Name;
        /// <summary>
        /// 该技能的描述
        /// </summary>
        public string Description = "";
        /// <summary>
        /// 表示该Skill的值基准，可为攻击力或生命上限
        /// </summary>
        public required ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 表示该Skill作用对象
        /// </summary>
        public required TargetType Target { get; set; }
        /// <summary>
        /// 表示该Skill附带的Buff
        /// </summary>
        public required Buff[] Effect { get; set; }
        /// <summary>
        /// 倍数，造成相当于自身攻击力Value倍的伤害；为0时，仅生效Buff；不为0时，造成伤害的同时给目标附加Buff；
        /// Value为正数时，造成伤害；Value为负数时，造成回复效果
        /// </summary>
        public required float Value { get; set; }
        /// <summary>
        /// Skill冷却所需轮数
        /// </summary>
        public required int CoolDown;
        public Skill()
        {

        }

    }
    internal class SkillCollection : IEnumerable
    {
        List<Skill> Skills = new();
        Dictionary<Skill, float> CoolDownList = new();
        public float CoolDownRatio = 1.0F;
        public int Count => Skills.Count;
        public Skill this[int index]
        {
            get { return Skills[index]; }
        }
        public Skill[] this[ReleaseType ReleaseType]
        {
            get
            {
                return Skills.Where(skill =>
                {
                    if (skill.ReleaseType == ReleaseType)
                        return true;
                    return false;
                }).ToArray();
            }
        }
        /// <summary>
        /// 添加新Skill
        /// </summary>
        /// <param name="newSkill"></param>
        public void Add(Skill newSkill)
            => Skills.Add(newSkill);
        /// <summary>
        /// 获取该Skill是否处于CD状态
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool InCoolDown(Skill skill)
            => CoolDownList.ContainsKey(skill);
        public int GetCoolDownRound(Skill skill)
        {
            if (InCoolDown(skill))
                return (int)CoolDownList[skill];
            else
                return 0;
        }
        /// <summary>
        /// 将目标Skill设置为CD状态
        /// </summary>
        /// <param name="skill"></param>
        public void ToCoolDown(Skill skill)
        {
            CoolDownList.Add(skill, skill.CoolDown);
        }
        /// <summary>
        /// 刷新Skill剩余CD轮数
        /// </summary>
        public void NextRound()
        {
            foreach (var skill in CoolDownList.Keys)
                if ((CoolDownList[skill] -= CoolDownRatio)<= 0)
                    CoolDownList.Remove(skill);
        }
        public int IndexOf(Skill skill)
            => Skills.IndexOf(skill);
        public Skill[] GetSkills() => Skills.ToArray();

        public Skill[] GetAvailableSkills() => Skills.Where(skill => !CoolDownList.ContainsKey(skill)).ToArray();
        public IEnumerator GetEnumerator()
            => ((IEnumerable)Skills).GetEnumerator();
        public void ForEach(Action<Skill> action)
            => Skills.ForEach(action);
        public string Serialize()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            return GetBase64Str(JsonSerializer.Serialize(Skills, options));
        }
        public void Deserialize(string serializeStr)
        {
            var _serializeStr = Base64ToStr(serializeStr);
            Skills = JsonSerializer.Deserialize<List<Skill>>(_serializeStr);
        }
    }
}

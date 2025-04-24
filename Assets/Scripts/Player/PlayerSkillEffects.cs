using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillEffects : MonoBehaviour
{
    [Header("Skill Effects")]
    public GameObject HamerSkill;
    public GameObject KickSkill;
    public GameObject SpellCastSkill;
    public GameObject HealSkill;
    public GameObject ShieldSkill;
    public GameObject ComboSkill;
    [Header("Skill Transforms")]
    public Transform KickTransform;
    public Transform SpellCastTransform;
    public Transform HamerSkillTransform;
    public Transform ComboSkillTransform;


    void HamerSkillCast()
    {
        Instantiate(HamerSkill, HamerSkillTransform.position, Quaternion.identity);
    }
    void KickSpellCast()
    {
        Instantiate(KickSkill, KickTransform.position, Quaternion.identity);
    }
    void SpellCast()
    {
        Instantiate(SpellCastSkill, SpellCastTransform.position, Quaternion.identity);  
    }
    void SlashComboCast()
    {
        Instantiate(ComboSkill, ComboSkillTransform.position, Quaternion.identity);
    }

    void ShieldCast()
    {
        Vector3 pos = transform.position;
    
        GameObject ShieldClone = Instantiate(ShieldSkill,pos, Quaternion.identity);
        ShieldClone.transform.SetParent(transform);
    }
    void HealCast()
    {
        Vector3 pos = transform.position;

        GameObject HealClone = Instantiate(HealSkill, pos, Quaternion.identity);
        HealClone .transform.SetParent(transform);
    }

}

namespace Audio
{
     public enum BGMID
     {
         GAMEBGM,  //<GameBGM
     }

     public enum SEID
     {
         DAMAGE,  //<Damage
         DECISION,  //<Decision
         ENEMYAPPEARANCE,  //<EnemyAppearance
         ENEMYATTACK,  //<EnemyAttack
         ENEMYHIT,  //<EnemyHit
         ENEMYSIREN,  //<EnemySiren
         PLAYERSTRENGTHATTACK,  //<PlayerStrengthAttack
         PLAYERWEAKATTACK,  //<PlayerWeakAttack
     }
}

public class BGMAudioData
{
     public string label;
     public Audio.BGMID id;
}

public class SEAudioData
{
     public string label;
     public Audio.SEID id;
}

public class AudioManager
{
     static public SEAudioData []SEData = new SEAudioData[]
     {
         new SEAudioData(){label = "Damage",         id = Audio.SEID.DAMAGE}, 
         new SEAudioData(){label = "Decision",         id = Audio.SEID.DECISION}, 
         new SEAudioData(){label = "EnemyAppearance",         id = Audio.SEID.ENEMYAPPEARANCE}, 
         new SEAudioData(){label = "EnemyAttack",         id = Audio.SEID.ENEMYATTACK}, 
         new SEAudioData(){label = "EnemyHit",         id = Audio.SEID.ENEMYHIT}, 
         new SEAudioData(){label = "EnemySiren",         id = Audio.SEID.ENEMYSIREN}, 
         new SEAudioData(){label = "PlayerStrengthAttack",         id = Audio.SEID.PLAYERSTRENGTHATTACK}, 
         new SEAudioData(){label = "PlayerWeakAttack",         id = Audio.SEID.PLAYERWEAKATTACK}, 
     };

     static public BGMAudioData []BGMData = new BGMAudioData[]
     {
         new BGMAudioData(){label ="GameBGM",          id = Audio.BGMID.GAMEBGM}, 
     };
}

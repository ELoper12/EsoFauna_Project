using UnityEngine;

public class EsoFauna_base
{
    public int index { get; private set; }//게임 내부에서 관리하기 위한 인덱스
    string monsterCode;//최초 발견 시에 부여되는 코드, 포획 이후 마핵 분석이 어느 정도 끝난 이후에는 개체명name이 붙게 됨
    string name;
    RankType rank;
       
    float Minsize = 1f;
    float Maxsize = 1f;//범위 계산해서 도감에 표시가 되죠; 

    //몸 색상
    Color[] mainColorlist;
    Color[] subColorlist;

    //식성 및 선호 먹이
    Food favoriteFood;
    Food[] appetite;
    
    //속성
    ElementType FirstElement;
    ElementType SecondElement;


    public enum Gender {Male, Female};
    public enum ElementType { Fire, Water, Ground, Wind, Plant, icy, Elecrtic, Neutral, Light, Dark};
    public enum RankType { Origin, Lesser, Greater, Apex, Mythic } // 근원종, 하위,상위,최상위, 신화종


    //이거는 엔티티만 필요
    Gender gender;
    float speed = 1f;
    float health = 1f;
    float damage;
    //개체 데이터
    //스트레스
    float stres; 
}

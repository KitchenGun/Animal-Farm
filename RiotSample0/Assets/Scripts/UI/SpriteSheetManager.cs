using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteSheetManager : MonoBehaviour
{
    //스프라이트 시트에 포함된 스프라이트를 캐시하는 딕셔너리
    //private static Dictionary<string, Dictionary<string, Sprite>> spriteSheets =
    //   new Dictionary<string, Dictionary<string, Sprite>>();
    private static Dictionary<string, SpriteAtlas> spriteSheets =
        new Dictionary<string, SpriteAtlas>();


    //스프라이트 시트에 포함된 스프라이트를 읽어서 캐시하는 메서드
    public static void Load(string path)
    {
        //if(!spriteSheets.ContainsKey(path))
        //{
        //    spriteSheets.Add(path, new Dictionary<string, Sprite>());
        //}
        ////스프라이트를 읽어서 이름과 관련지어서 캐시한다
        //Sprite[] sprites = Resources.LoadAll<Sprite>(path);
        //foreach(Sprite sprite in sprites)
        //{
        //    if (!spriteSheets[path].ContainsKey(sprite.name))
        //    {
        //        spriteSheets[path].Add(sprite.name, sprite);
        //    }
        //}
        //Debug.Log("sprites count" + sprites.Length);

        if(!spriteSheets.ContainsKey(path))
        {
            SpriteAtlas altlas = Resources.Load<SpriteAtlas>(path);//불러오기
            if(altlas!=null)
            {
                spriteSheets.Add(path, altlas);
            }
        }

    }

    public static Sprite GetSpriteByName(string path,string name)
    {
        //if(spriteSheets.ContainsKey(path)&&spriteSheets[path].ContainsKey(name))
        if (spriteSheets.ContainsKey(path) && spriteSheets[path].GetSprite(name))
        {
            return spriteSheets[path].GetSprite(name);
        }
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
   public string[] mensajes = new string [5];
   private int actualIndex = 0;
   public static TextWriter instance;
   private List<TextWriterSingle> textWriterSingleList;
   public TMP_Text textBox;

   private void Awake()
   {
      instance = this;
      textWriterSingleList = new List<TextWriterSingle>();
   }

   public bool credits;
   private void Start()
   {
      if (credits)
      {
         instance = this;
         AudioManager.instance.Play("PencilWrite");
         AddWriter_Static(textBox, System.Text.RegularExpressions.Regex.Unescape(mensajes[0]), .1f, true);
      }
   }

   public void NextText()
   {
      AddWriter_Static(textBox, mensajes[actualIndex], .05f, true);
      AudioManager.instance.Play("PencilWrite");
      actualIndex++;
   }
   
   public static void AddWriter_Static(TMP_Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
   {
      instance.AddWriter(uiText, textToWrite, timePerCharacter, invisibleCharacters);
   }
   private void AddWriter(TMP_Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
   {
      textWriterSingleList.Add(new TextWriterSingle(uiText, textToWrite, timePerCharacter, invisibleCharacters));
   }

   private void Update()
   {
      for (int i = 0; i < textWriterSingleList.Count; i++)
      {
         bool destroyInstance = textWriterSingleList[i].Update();
         if (destroyInstance)
         {
            textWriterSingleList.RemoveAt(i);
            i--;
         }
      }
   }

   public void FuncionFinal()
   {
      StartCoroutine(FuncionFinalIE());
      AudioManager.instance.Stop("PencilWrite");
   }

   IEnumerator FuncionFinalIE()
   {
      if (!credits)
      {
            yield return new WaitForSeconds(2f);
            GameManager.Instance.BlinkEye(GameManager.BlinkEvent.NEXTLEVEL);
            yield return new WaitForSeconds(1f);
            textBox.text = "";
      }
      if(credits)
      {
         yield return new WaitForSeconds(3f);
         GameManager.Instance.BlinkEye(GameManager.BlinkEvent.ENDGAME);
      }
   }

   //Una sola instancia de TextWriter
   
   
   public class TextWriterSingle
   {
      private TMP_Text uiText;
      private string textToWrite;
      private int characterIndex;
      private float timePerCharacter;
      private float timer;
      private bool invisibleCharacters;
      
      public TextWriterSingle(TMP_Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
      {
         this.uiText = uiText;
         this.textToWrite = textToWrite;
         this.timePerCharacter = timePerCharacter;
         this.invisibleCharacters = invisibleCharacters;
         characterIndex = 0;
      }
      public bool Update()
      {
         timer -= Time.deltaTime;
            if (timer <= 0f)
            {
               //Muestra el siguiente caracter
               timer += timePerCharacter;
               characterIndex++;
               string text = textToWrite.Substring(0, characterIndex);
               if (invisibleCharacters)
               {
                  text +="<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
               }
               uiText.text = text;

               if (characterIndex >= textToWrite.Length)
               {
                  //Revisamos si mostrar la frase completa
                  instance.FuncionFinal();
                  return true;
               }
            }
         return false;
      }
   }

   
   
}

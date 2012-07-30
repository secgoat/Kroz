using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    public class AncientTablet : BaseItem
    {
        int m_Level;
        List<String> m_Message;

        public AncientTablet(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame, int level)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {  
            this.m_Level = level;
            this.m_Description = "You find an ancient tablet with the inscription:";
            this.m_Message = new List<String>();
            this.setMessage();
           
        }
        
    public List<String> Message { get { return m_Message; } }
        
    private void setMessage()
    {
        this.m_Message.Add(m_Description);
        //this.m_Message.Add(" ");
        if (m_Level == 1)
            this.m_Message.Add("Remember to experiment with every new object on a level.");
        if (m_Level == 2)//3
            this.m_Message.Add("Only use your valuable Teleports for last chance escapes!");
        if (m_Level == 3)//5
            this.m_Message.Add("You're right in the middle of a Lava Flow! Run for it!");
        if (m_Level == 4)//7
            this.m_Message.Add("You'll need the two keys from the previous level!");
        if (m_Level == 5)//9
            this.m_Message.Add("The two chests can be yours if you find the hidden spell!");
        if (m_Level == 6)//11
            this.m_Message.Add("You learn from successful failures.");
        if (m_Level == 7)//13
            this.m_Message.Add("A Creature Generator exists within this chamber--destroy it!");
        if (m_Level == 8)//15
            this.m_Message.Add("By throwing dirt at someone you only lose ground.");
        if (m_Level == 9)//17
            this.m_Message.Add("The Bubble Creatures knock off three Gems when touched!");
        if (m_Level == 10)//19
            this.m_Message.Add("Be vigilant Adventurer, the Crown is near, but well protected.");
        if (m_Level == 11)//20
            this.m_Message.Add("You\'ve survived so far, Adventurer. Can you succeed?");
        //this.m_Message.Add(" ");
        this.m_Message.Reverse();

    
    }

    public override void OnCollision(Player player)
    {
        player.Score = 1000;
    } 
  }
}

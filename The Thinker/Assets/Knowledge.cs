using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leguar.TotalJSON;

public class Knowledge : MonoBehaviour
{
    public static Knowledge knowledge;
    public Taxonomy taxonomy;
    public Relations relations;
    public Distributions distributions;
    public Discrete_Probabilites discrete_probabilities;
    public static bool isAwaiting = true;
    

    public static void ParseResponse(string text){
        
        knowledge = JSON.ParseString(text).Deserialize<Knowledge>();
        
        isAwaiting = false;
        
    }

    
    



    

    public class Taxonomy
    {
        public Dictionary<string,string> children;
        public Dictionary<string,string> parents;
    }

    public class Relations
    {
        public Dictionary<string, string> entity;
        public Dictionary<string, string> relationship;
        public Dictionary<string, int> distribution;
    }

    public class Distributions
    {
        
        public Dictionary<string, int> id;
        public Dictionary<string, string> type;
        public Dictionary<string, float> mean;
        public Dictionary<string, float> variance;
        public Dictionary<string, string> discrete_id;
    }

    

    public class Discrete_Probabilites
    {
        public Dictionary<string, string> discrete_id;
        public Dictionary<string, string>  value;
        public Dictionary<string, string> value_type;
        public Dictionary<string, float> probabilitiies; 


    }


}



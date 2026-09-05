using UnityEngine;

public class FormesDrop : MonoBehaviour
{
    string formeDeDrop = "glace";
    bool temperatureHaute = false;

    void Start()
    {
        switch (formeDeDrop)
        {
            case "liquide":
                Debug.Log("Drop s'infiltre dans les fissures.");
                break;
            case "glace":
                Debug.Log("Drop cogne les blocs gelés.");
                if (temperatureHaute)
                {
                    Debug.Log("Mais Drop fond !");
                }
                break;
            case "vapeur":
                Debug.Log("Drop s'élève dans les airs.");
                break;
            default:
                Debug.Log("Forme inconnue !");
                break;
        }
    }
}

/*using UnityEngine;

public class boolLumo : MonoBehaviour
{

    string formeDeDrop = "glace";
    bool temperatureHaute;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

            switch (formeDeDrop)
            {
                case "liquide":
                    Debug.Log("Drop s'infiltre dans les fissures.");
                    break;
                case "glace":
                    Debug.Log("Drop cogne les blocs gelés.");
                    if(temperatureHaute)Debug.Log("Drop fond");
                    break;
                case "vapeur":
                    Debug.Log("Drop s'élève dans les airs.");
                    break;
                default:
                    Debug.Log("Forme inconnue !");
                    break;
            }
    }

}*/

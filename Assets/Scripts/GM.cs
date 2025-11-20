using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Advertisements;

public enum AtomicNumber
{
    H = 1,
    H2= 2,
    H3 = 3,
    H4 = 4,
    C = 6,
    Ne = 10,
    O = 8,
    Si = 14,
    Fe = 26
}

public enum GameState
{
    Playing,
    Moving,
    GameOver
}

public class GM : MonoBehaviour
{
    public GameState state = GameState.GameOver;
    public PostManager postManager;
    public SpectralManager spectralManager;
    public ShellController shellController;
    public AdsManager adsManager;
    public float delay;
    private bool moveMade;
    public int newElement = 1;
    private bool[] lineMoveFin = new bool[4] { true, true, true, true};

    private Tile[,] AllTiles = new Tile[4, 4];
    private List<Tile[]> cols = new List<Tile[]>();
    private List<Tile[]> rows = new List<Tile[]>();
    private List<Tile> EmptyTiles = new List<Tile>();

    public int combo;
    public Text text;
    public Text FELText;

    //private string playStoreID = "3577361";
    //private string rewardAd = "rewardedVideo";

    void Start()
    {
        //Advertisement.Initialize(playStoreID);
    }

    public void Create()
    {
        FELText.text = "FELTs: " + PlayerPrefs.GetInt("FELT", 0).ToString();
        newElement = 1;
        if(state == GameState.GameOver)
        {

            Tile[] tiles = FindObjectsOfType<Tile>();

            foreach (Tile t in tiles)
            {
                t.Number = 0;
                t.Iron = false;
                AllTiles[t.indRow, t.indCol] = t;
                EmptyTiles.Add(t);
            }

            shellController.ResetShells();
            combo = 0;
            cols.Clear();
            rows.Clear();

            #region Rows and Cols Definition
            cols.Add(new Tile[] { AllTiles[0, 0], AllTiles[1, 0], AllTiles[2, 0], AllTiles[3, 0]});
            cols.Add(new Tile[] { AllTiles[0, 1], AllTiles[1, 1], AllTiles[2, 1], AllTiles[3, 1]});
            cols.Add(new Tile[] { AllTiles[0, 2], AllTiles[1, 2], AllTiles[2, 2], AllTiles[3, 2]});
            cols.Add(new Tile[] { AllTiles[0, 3], AllTiles[1, 3], AllTiles[2, 3], AllTiles[3, 3]});

            rows.Add(new Tile[] { AllTiles[0, 0], AllTiles[0, 1], AllTiles[0, 2], AllTiles[0, 3]});
            rows.Add(new Tile[] { AllTiles[1, 0], AllTiles[1, 1], AllTiles[1, 2], AllTiles[1, 3]});
            rows.Add(new Tile[] { AllTiles[2, 0], AllTiles[2, 1], AllTiles[2, 2], AllTiles[2, 3]});
            rows.Add(new Tile[] { AllTiles[3, 0], AllTiles[3, 1], AllTiles[3, 2], AllTiles[3, 3]});
            #endregion

            Generate();
            Generate();
            state = GameState.Playing;
        }
    }

    bool MoveDownIndex(Tile[] LineOfTiles)
    {
        for (int i = 0; i < LineOfTiles.Length - 1; i++)
        {
            //Moving
            if(LineOfTiles[i].Number == 0 && LineOfTiles[i+1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i + 1].Number;
                LineOfTiles[i + 1].Number = 0;
                return true;
            }

            //Fusing
            if(LineOfTiles[i].Number != 0 && LineOfTiles[i].merged == false && LineOfTiles[i+1].merged == false && LineOfTiles[i].Number == LineOfTiles[i + 1].Number)
            {
                LineOfTiles[i].Number++;
                LineOfTiles[i + 1].Number = 0;
                LineOfTiles[i].Merge();
                combo++;
                return true;
            }
        }
        return false;
    }

    bool MoveUpIndex(Tile[] LineOfTiles)
    {
        for (int i = LineOfTiles.Length - 1; i > 0; i--)
        {
            //Moving
            if (LineOfTiles[i].Number == 0 && LineOfTiles[i - 1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i - 1].Number;
                LineOfTiles[i - 1].Number = 0;
                return true;
            }

            //Fusing
            if (LineOfTiles[i].Number != 0 && LineOfTiles[i].merged == false && LineOfTiles[i-1].merged == false && LineOfTiles[i].Number == LineOfTiles[i - 1].Number)
            {
                LineOfTiles[i].Number++;
                LineOfTiles[i - 1].Number = 0;
                LineOfTiles[i].Merge();
                combo++;
                return true;
            }
        }
        return false;
    }

    public void UpdateEmptyTiles()
    {
        text.text = "Fused: " + combo.ToString();
        EmptyTiles.Clear();
        foreach  (Tile t in AllTiles)
        {
            if(t.Number == 0)
            {
                EmptyTiles.Add(t);
            }
            t.merged = false;
            
            if(t.Iron == true)
            {
                EndGame();
            }
        }
    }

    public void Shift(Dir d)
    {
        if (delay > 0)
        {
            StartCoroutine(ShiftCo(d));
        }
        else
        {
            for (int i = 0; i < rows.Count; i++)
            {
                switch (d)
                {
                    case Dir.Right:
                        while (MoveUpIndex(rows[i])) { moveMade = true; }
                        break;
                    case Dir.Left:
                        while (MoveDownIndex(rows[i])) { moveMade = true; }
                        break;
                    case Dir.Up:
                        while (MoveDownIndex(cols[i])) { moveMade = true; }
                        break;
                    case Dir.Down:
                        while (MoveUpIndex(cols[i])) { moveMade = true; }
                        break;
                }
            }
            if (moveMade)
            {
                UpdateEmptyTiles();
                Generate();
            }
        }
    }

    IEnumerator ShiftCo(Dir d)
    {
        state = GameState.Moving;
        moveMade = false;
        postManager.Bloom(1f);
        for (int i = 0; i < rows.Count; i++)
        {
            switch (d)
            {
                case Dir.Right:
                    for (i = 0; i < rows.Count; i++)
                        StartCoroutine(MoveUpIndexCo(rows[i], i));
                    break;
                case Dir.Left:
                    for (i = 0; i < rows.Count; i++)
                        StartCoroutine(MoveDownIndexCo(rows[i], i));
                    break;
                case Dir.Up:
                    for (i = 0; i < cols.Count; i++)
                        StartCoroutine(MoveDownIndexCo(cols[i], i));
                    break;
                case Dir.Down:
                    for (i = 0; i < cols.Count; i++)
                        StartCoroutine(MoveUpIndexCo(cols[i], i));
                    break;
            }
        }
        while (!(lineMoveFin[0] && lineMoveFin[1] && lineMoveFin[2] && lineMoveFin[3]))
            yield return null;

        if (moveMade)
        {
            UpdateEmptyTiles();
            Generate();

            if(!CanMove())
            {
                EndGame();
            }
        }

        if (state == GameState.Moving)
        {
            state = GameState.Playing;
        }
        postManager.Bloom(0f);
        StopAllCoroutines();
    }

    IEnumerator MoveUpIndexCo(Tile[] line, int index)
    {
        lineMoveFin[index] = false;
        while (MoveUpIndex(line))
        {
            yield return new WaitForSeconds(delay);
            moveMade = true;
        }
        lineMoveFin[index] = true;
    }

    IEnumerator MoveDownIndexCo(Tile[] line, int index)
    {
        lineMoveFin[index] = false;
        while (MoveDownIndex(line))
        {
            yield return new WaitForSeconds(delay);
            moveMade = true;
        }
        lineMoveFin[index] = true;
    }

    public void Generate()
    {
        if(EmptyTiles.Count > 0)
        {
            int indRand = Random.Range(0, EmptyTiles.Count);
            int heliNum = Random.Range(0, 10);
            EmptyTiles[indRand].Number = newElement;
            EmptyTiles[indRand].Appear();
            EmptyTiles.RemoveAt(indRand);
        }
    }

    public void FELT(bool menuButton)
    {
        int feltnumber = PlayerPrefs.GetInt("FELT", 0);
        if(!menuButton)
        {
            if(PlayerPrefs.GetInt("FELT", 0) == 0)
            {
                //Advertisement.Show(rewardAd);
                adsManager.ShowRewarded();
            }
            else
            {
                PlayerPrefs.SetInt("FELT", feltnumber-=1);
            }
            foreach(Tile t in AllTiles)
            {
                if(t.Number == newElement)
                {
                    t.Number = 0;
                    EmptyTiles.Add(t);
                }
            }
            newElement++;
            int emptyCount = 0;
            foreach (Tile t in AllTiles)
            {
                if(t.Number != 0)
                    emptyCount++;
            }
            if (emptyCount == 0)
                EndGame();
        }
        else
        {
            adsManager.ShowRewarded();
            PlayerPrefs.SetInt("FELT", feltnumber+=1);
            Debug.Log(PlayerPrefs.GetInt("FELT", 0));
        }
        FELText.text = "FELTs: " + PlayerPrefs.GetInt("FELT", 0).ToString();
    }
    public bool CanMove()   
    {
        if(EmptyTiles.Count == 0)
        {
            postManager.ChromaticAbberation(0.3f);
            for (int i = 0; i < cols.Count; i++)
            {
                for (int j = 0; j < rows.Count-1; j++)
                {
                    if(AllTiles[j,i].Number == AllTiles[j+1,i].Number)
                        return true;
                }
            }
            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < cols.Count-1; j++)
                {
                    if(AllTiles[i,j].Number == AllTiles[i,j+1].Number)
                        return true;
                }
            }
            return false;
        }
        postManager.ChromaticAbberation(0f);
        return true;
    }
    public void EndGame()
    {
        state = GameState.GameOver;
        spectralManager.GameOver();
        postManager.Bloom(5);
    }
}

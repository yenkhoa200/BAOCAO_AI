using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI.Table;

public class Puzzle_Board : MonoBehaviour
{
    //Start() chạy 1 lần duy nhất khi GameObject được khởi tạo.
    public GameObject TilePrefab;
    public List<Texture> puzzleImg= new List<Texture>();
    // Chỉ số ảnh hiện tại
    private int currentTextureIndex = 0;
    // Danh sách các ô puzzle được tạo ra
    private List<GameObject> tiles=new List<GameObject>();
    //// Vị trí của 9 ô trên bàn
    private List<Vector3> tilesLocation = new List<Vector3>()
    {
        new Vector3(-1.0f, 1.0f, 0.0f),
        new Vector3( 0.0f, 1.0f, 0.0f),
        new Vector3( 1.0f, 1.0f, 0.0f),

        new Vector3(-1.0f, 0.0f, 0.0f),
        new Vector3( 0.0f, 0.0f, 0.0f),
        new Vector3( 1.0f, 0.0f, 0.0f),

        new Vector3(-1.0f, -1.0f, 0.0f),
        new Vector3( 0.0f, -1.0f, 0.0f),
        new Vector3( 1.0f, -1.0f, 0.0f),
    };
    void Start()
    {
        CreateTiles();
        Init();
    }
    private void Init()
    {
        SetTexture();

    }
    // Tạo 9 ô puzzle
    void CreateTiles()
    {
        for (int i = 0; i < tilesLocation.Count; i++)
        {
            GameObject tile = Instantiate(TilePrefab);

            tile.name=i.ToString();
            tile.transform.parent = transform;
            tiles.Add(tile);
            tiles[i].transform.position = tilesLocation[i];
        }
    }
    //Cắt ảnh thành 9 phần và gán cho các ô
    void SetTexture()
    {
        Texture mainTexture = puzzleImg[currentTextureIndex];
        int numRows = 3;
        int tileSize=mainTexture.width/numRows;
        for (int i = 0; i < 8; i++)
        {
            GameObject tile = tiles[i];
            Renderer renderer = tile.GetComponent<Renderer>();
            Material material = renderer.material;

            // Calculate the texture coordinates.
             int row = i / numRows;
             int col = i % numRows;

             float xMin = col * (float)tileSize / mainTexture.width;
            float yMin = 1.0f - (row + 1) * (float)tileSize / mainTexture.height;
        

            material.mainTexture = mainTexture;
            material.mainTextureScale = new Vector2((float)tileSize / mainTexture.width,
                (float)tileSize / mainTexture.height);
            material.mainTextureOffset = new Vector2(xMin, yMin);
        }
        // Ô cuối là ô trống
        tiles[8].GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        // tiles[8].GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        
    }
    //Nhấm phím 'N' để chuyển sang hình ảnh tiếp theo trong danh sách puzzleImg.
    public void NextImage()
    {
        Debug.Log("NextImage.");
        currentTextureIndex++;
        if (currentTextureIndex == puzzleImg.Count)
        {
            currentTextureIndex = 0;
        }
        Init();
    }
    //Update() chạy liên tục mỗi frame (khoảng 60 lần/giây hoặc hơn)
    void Update()
    {

        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            NextImage();
        }
    }
}

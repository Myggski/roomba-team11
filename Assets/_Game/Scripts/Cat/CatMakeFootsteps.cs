using UnityEngine;
using Random = UnityEngine.Random;

public class CatMakeFootsteps : MonoBehaviour {
    [SerializeField]
    private PawPrintChoreItemSet pawPrintChoreItemSet;
    [SerializeField] 
    private CatVariables _catVariables;
    
    private float _nextLeaveFootprints;
    private Vector3 _recentFootprintPos;
    private int _footprintsMade;
    private int _footprintsToMake;
    private int _totalPawPrintsSpawned;

    private Vector3 RightPawOffset => new Vector3(0.225f, 0.1f, 0.2f);
    private Vector3 LeftPawOffset => new Vector3(-0.225f, 0.1f, 0);

    public void Start()
    {
        PawPrintObjectPool.Instance.SetupPool();
        SetupFootprints();
    }

    public void MakeFootprints()
    {
        if ((_totalPawPrintsSpawned * 2) >= PawPrintObjectPool.Instance.TotalItemsToSpawn)
        {
            _catVariables.stopMakeFootstepsEvent?.Call();
            enabled = false;
            return;
        }

        if (Time.time < _nextLeaveFootprints) return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            if (Vector3.Distance(_recentFootprintPos, transform.position) > 0.6f && _footprintsMade < _footprintsToMake)
            {
                SpawnPawPrints(hit.point, transform.rotation);
                
                _footprintsMade++;
            }

            if (_footprintsMade >= _footprintsToMake)
            {
                SetupFootprints();
            }
        }
    }

    private void SpawnPawPrints(Vector3 position, Quaternion rotation)
    {
        PawPrintChoreItem rightPawprint = PawPrintObjectPool.Instance.Get();
        rightPawprint.transform.rotation = rotation;
        rightPawprint.transform.position = position;
        rightPawprint.transform.localPosition += RightPawOffset;
        rightPawprint.gameObject.SetActive(true);
        rightPawprint.name = "right paw";

        PawPrintChoreItem leftPawprint = PawPrintObjectPool.Instance.Get();
        leftPawprint.transform.rotation = rotation;
        leftPawprint.transform.position = position;
        leftPawprint.transform.localPosition += LeftPawOffset;
        leftPawprint.gameObject.SetActive(true);
        leftPawprint.name = "left paw";
        
        _recentFootprintPos = position;
        _totalPawPrintsSpawned++;
    }
    
    private void SetupFootprints()
    {
        _nextLeaveFootprints = Time.time + Random.Range(_catVariables.delayBetweenFootprints.x,_catVariables.delayBetweenFootprints.y);
        _footprintsToMake = (int)Random.Range(_catVariables.amountFootstepsToLeave.x,_catVariables.amountFootstepsToLeave.y);
        
        if ((_footprintsToMake + _totalPawPrintsSpawned) * 2 > pawPrintChoreItemSet.NumberOfChoreItems) {
            _footprintsToMake = Mathf.RoundToInt(((PawPrintObjectPool.Instance.TotalItemsToSpawn - (_totalPawPrintsSpawned * 2)) / 2f) - 0.5f);

            if (_footprintsToMake == 0) {
                enabled = false;
            } 
        }
        
        _footprintsMade = 0;
    }
}

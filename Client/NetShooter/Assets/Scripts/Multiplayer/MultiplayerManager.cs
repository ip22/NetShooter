using Colyseus;
using UnityEngine;
using System.Collections.Generic;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [field: SerializeField] public Skins skins;
    [field: SerializeField] public LossCounter lossCounter { get; private set; }
    [field: SerializeField] public SpawnPoints spawnPoints { get; private set; }

    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private EnemyController _enemy;

    private ColyseusRoom<State> _room;
    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();

    protected override void Awake() {
        base.Awake();
        Instance.InitializeClient();
        Connect();
    }

    private async void Connect() {
        spawnPoints.GetPoint(Random.Range(0, spawnPoints.length), out Vector3 spawnPosition, out Vector3 spawnRotation);

        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"skins", skins.length },
            {"points", spawnPoints.length },
            {"hp", _player.maxHealth},
            {"speed", _player.speed},
            {"pX", spawnPosition.x},
            {"pY", spawnPosition.y},
            {"pZ", spawnPosition.z},
            {"rY", spawnRotation.y}
        };

        _room = await Instance.client.JoinOrCreate<State>("state_handler", data);

        _room.OnStateChange += OnChange;

        _room.OnMessage<string>("Shoot", ApplyShoot);

        _room.OnMessage<string>("Gun", SetGun);
    }

    private void SetGun(string jsonGunInfo) {
        GunInfo gunInfo = JsonUtility.FromJson<GunInfo>(jsonGunInfo);
        if (_enemies.ContainsKey(gunInfo.id) == false) {
            Debug.LogError("Enemy trying to set gun, but he's not exists.");
            return;
        }

        _enemies[gunInfo.id].SetGun(gunInfo);
    }

    private void ApplyShoot(string jsonShootInfo) {
        ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);

        if (_enemies.ContainsKey(shootInfo.key) == false) {
            Debug.LogError("Enemy trying to shoot, but he's not exists.");
            return;
        }

        _enemies[shootInfo.key].Shoot(shootInfo);
    }

    private void OnChange(State state, bool isFirstState) {
        if (isFirstState == false) return;

        state.players.ForEach((key, player) => {
            if (key == _room.SessionId) CreatePlayer(player);
            else CreateEnemy(key, player);
        });

        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnRemove += RemoveEnemy;
    }

    private void CreatePlayer(Player player) {
        var position = new Vector3(player.pX, player.pY, player.pZ);

        var rotation = Quaternion.Euler(0, player.rY, 0);

        var playerCharacter = Instantiate(_player, position, rotation);
        player.OnChange += playerCharacter.OnChange;

        _room.OnMessage<int>("Restart", playerCharacter.GetComponent<Controller>().Restart);

        playerCharacter.GetComponent<SetSkin>().Set(skins.GetMaterial(player.skin));
    }

    private void CreateEnemy(string key, Player player) {
        var position = new Vector3(player.pX, player.pY, player.pZ);

        var enemy = Instantiate(_enemy, position, Quaternion.identity);
        enemy.Init(key, player);
        enemy.GetComponent<SetSkin>().Set(skins.GetMaterial(player.skin));

        _enemies.Add(key, enemy);

    }

    private void RemoveEnemy(string key, Player player) {
        if (_enemies.ContainsKey(key) == false) return;

        var enemy = _enemies[key];
        enemy.Destroy();

        _enemies.Remove(key);
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        _room?.Leave();
    }

    public void SendMessage(string key, Dictionary<string, object> data) {
        _room?.Send(key, data);
    }

    public void SendMessage(string key, string data) {
        _room?.Send(key, data);
    }

    public string GetSessionID() => _room.SessionId;

}

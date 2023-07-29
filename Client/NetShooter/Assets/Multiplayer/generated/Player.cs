// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.46
// 

using Colyseus.Schema;

public partial class Player : Schema {
	[Type(0, "number")]
	public float x = default(float);

	[Type(1, "number")]
	public float y = default(float);

    // ** DOP
    // добавил две переменные для пересылки серверу
    [Type(2, "number")]
    public float h = default(float);

    [Type(3, "number")]
    public float v = default(float);
    //**
}


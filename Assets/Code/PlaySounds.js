#pragma strict

var sound : AudioClip;

function Start () {

	AudioSource.PlayClipAtPoint(sound, transform.position);

}

function Update () {

}
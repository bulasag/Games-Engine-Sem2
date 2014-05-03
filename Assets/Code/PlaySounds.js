#pragma strict

var boostsound : AudioClip;

function Start () {

	AudioSource.PlayClipAtPoint(boostsound, transform.position);

}

function Update () {

}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LightScript : MonoBehaviour {
	public GameObject light;
	const int width = 128;
	const int height = 128;
	//int[][] field = Enumerable.Repeat<int[]>((Enumerable.Repeat<int>(0,height).ToArray()),width).ToArray();
	int[,] field=new int[width,height];
	int[,] copy=new int[width,height];
	float time=0;

	void Start () {
		//for(int j=0;j<height;j++) field[height/2][j]=1;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				field[i,j]= i==width/2 ? 1 : 0;
				Instantiate (light, new Vector3(i,j,0),Quaternion.Euler(-90f,0f,0f)).name="light-"+i+"-"+j;
			}
		}
	}

	void Update () { if ((time += Time.deltaTime) >= 1) change ();}

	void change(){
		Debug.Log (time);
		time = 0;
		for(int i=0;i<width;i++){  //惑星のループ
			for(int j=0;j<height;j++){
				int count=0;
				for(int x=-1;x<=1;x++){  //衛星のループ
					for(int y=-1;y<=1;y++){
						if(x!=0 || y!=0){
							if(i+x>=0 && i+x<=width-1 && j+y>=0 && j+y<=height-1 ){
								if(field[i+x,j+y]>=1){
									count+=1;
								}
							}
						}
					}
				}
				copy[i,j]=next_state(field[i,j],count);
			}
		}
		for(int i=0;i<width;i++){
			for(int j=0;j<height;j++){
				GameObject.Find ("light-" + i + "-" + j).GetComponent<Renderer> ().material.color = new Color (0, field[i,j]*255, 0);
				field[i,j]=copy[i,j];
			}
		}
	}

	int next_state(int me, int other){
		if(other==3) return 1;
		else if(other==2) return me;
		else return 0;
	}
}

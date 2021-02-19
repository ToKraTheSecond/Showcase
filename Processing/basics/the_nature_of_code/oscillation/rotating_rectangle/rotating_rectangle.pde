float a = 0.0;
float aVelocity = 0.0;
float aAcceleration = 0.01;

void setu() {
  size(640,360);
}

void draw() {
  background(255);
  
  a += aVelocity;
  aVelocity += aAcceleration;
  
  rectMode(CENTER);
  stroke(0);
  fill(127);
  translate(width/2,height/2);
  rotate(a);
  rect(0,0,64,36);
}

Bubble[] bubbles = new Bubble[5];
int padding = 100;

void setup() {
  size(640,360);
  for (int i = 0; i < bubbles.length; i++) {
      bubbles[i] = new Bubble(random(10, 50)); 
  }
}

void draw() {
  background(255);
  for (int i = 0; i < bubbles.length; i++) {  
    bubbles[i].ascend();
    bubbles[i].display();
    bubbles[i].top();
  }
}

class Bubble {
  float x;
  float y;
  float diameter;
  
  Bubble(float tempD) {
    x = width/2;
    y = height;
    diameter = tempD;
  }
  
  void ascend() {
    y--;
  }
  
  void display() {
    stroke(0);
    fill(127);
    ellipse(x, y, diameter, diameter);
  }
  
  void top() {
    if (y < padding * -1) {
      y = height + padding;
    }
  }
}

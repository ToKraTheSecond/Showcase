Bubble b;
int padding = 100;

void setup() {
  size(640,360);
  b = new Bubble(55);
}

void draw() {
  background(255);
  b.ascend();
  b.display();
  b.top();
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

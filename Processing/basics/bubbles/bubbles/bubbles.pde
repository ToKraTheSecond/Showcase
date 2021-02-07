Bubble[] bubbles = new Bubble[1];
int padding = 50;

void setup() {
  size(640,360);
  for (int i = 0; i < bubbles.length; i++) {
      bubbles[i] = new Bubble(random(10, 50), int(random(0,width))); 
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

void mouseClicked() {
  bubbles = (Bubble[]) append(bubbles, new Bubble(random(10, 50), int(random(0,width))));
}
  

class Bubble {
  float x;
  float y;
  float diameter;
  
  Bubble(float Diameter, int VerticalPos) {
    x = VerticalPos;
    y = height;
    diameter = Diameter;
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

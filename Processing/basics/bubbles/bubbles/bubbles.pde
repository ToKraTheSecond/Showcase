Bubble[] bubbles = new Bubble[0];
int padding = 50;

void setup() {
  size(640,360);
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
  bubbles = (Bubble[]) append(bubbles, new Bubble(mouseX, mouseY, random(10, 50)));
}
  

class Bubble {
  float x;
  float y;
  float diameter;
  
  Bubble(int VerticalPos, int HorizontalPos, float Diameter) {
    x = VerticalPos;
    y = HorizontalPos;
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

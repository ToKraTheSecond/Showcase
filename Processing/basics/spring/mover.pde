// The Nature of Code; Daniel Shiffman

class Bob {
  PVector position;
  PVector velocity;
  PVector acceleration;
  float mass = 24;
  float damping = 0.98;
  PVector dragOffset;
  boolean dragging = false;
  
  Bob(float x, float y) {
    position = new PVector(x,y);
    velocity = new PVector();
    acceleration = new PVector();
    dragOffset = new PVector();
  }
  
  void update() {
    velocity.add(acceleration);
    velocity.mult(damping);
    position.add(velocity);
    acceleration.mult(0);
  }
  
  void applyForce(PVector force) {
    PVector f = force.get();
    f.div(mass);
    acceleration.add(f);
  }
  
  void display() {
    stroke(0);
    strokeWeight(2);
    fill(175);
    if (dragging) {
      fill(50);
    }
    ellipse(position.x, position.y, mass * 2, mass * 2);
  }
  
  void clicked(int mx, int my) {
    float d = dist(mx, my, position.x, position.y);
    if (d < mass) {
      dragging = true;
      dragOffset.x = position.x - mx;
      dragOffset.y = position.y - my;
    }
  }
  
  void stopDragging() {
    dragging = false;
  }
  
  void drag(int mx, int my) {
    if (dragging) {
      position.x = mx + dragOffset.x;
      position.y = my + dragOffset.y;
    }
  }
}

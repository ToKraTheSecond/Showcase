// Nature of Code 2011; Daniel Shiffman; Chapter 3: Oscillation

class Spring {
  PVector anchor;  
  float len;
  float k = 0.2;
  
  Spring(float x, float y, int l) {
    anchor = new PVector(x, y);
    len = l;
  }
  
  void connect(Bob b) {
    PVector force = PVector.sub(b.position, anchor);
    float d = force.mag();
    float stretch = d - len;
    // Calculate force according to Hooke's law
    // F = k * stretch
    force.normalize();
    force.mult(-1 * k * stretch);
    b.applyForce(force);
  }
  
  void constrainLength(Bob b, float minlen, float maxlen) {
    PVector dir = PVector.sub(b.position, anchor);
    float d = dir.mag();
    if (d < minlen) {
      dir.normalize();
      dir.mult(minlen);
      b.position = PVector.add(anchor, dir);
      b.velocity.mult(0);
    }
  }
  
  void display() {
    stroke(0);
    fill(175);
    strokeWeight(2);
    rectMode(CENTER);
    rect(anchor.x, anchor.y, 10, 10);
  }
  
  void displayLine(Bob b) {
    strokeWeight(2);
    stroke(0);
    line(b.position.x, b.position.y, anchor.x, anchor.y);
  }
}

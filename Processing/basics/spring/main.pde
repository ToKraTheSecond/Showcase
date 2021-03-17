// The Nature of Code; Daniel Shiffman

Bob bob;
Spring spring;

public void settings() {
  size(640, 360);
}

void setup() {
  spring = new Spring(width / 2, 10, 100);
  bob = new Bob(width / 2, 100);
}

void draw() {
  background(255);
  PVector gravity = new PVector(0, 2);
  bob.applyForce(gravity);

  spring.connect(bob);
  spring.constrainLength(bob, 30, 200);
  bob.update();
  bob.drag(mouseX, mouseY);
  spring.displayLine(bob);
  bob.display();
  spring.display();

  fill(0);
  text("click on bob to drag", 10, height - 5);
}

void mousePressed() {
  bob.clicked(mouseX, mouseY);
}

void mouseReleased() {
  bob.stopDragging();
}

var player;
var scl = 20; // Scale of the play feild
var food;

function Direction() {
}

function setup() {
    createCanvas(600, 600);
    Direction.NORTH = createVector(0,-1);
    Direction.EAST = createVector(1,0);
    Direction.WEST = createVector(-1,0);
    Direction.SOUTH = createVector(0,1);
    frameRate(10);
    food = pickLocation();
    player = new Snake();
}

function draw() {
    background(51);
    text(player.total,20,20)
    player.update();
    player.show();

    fill(255, 0, 200);

    if(player.eat(food)) {
        food = pickLocation()
    }
    
    rect(food.x, food.y, scl, scl);
}

function pickLocation() {
    var cols = floor(width/scl);
    var rows = floor(height/scl);
    return createVector(floor(random(cols)), floor(random(rows))).mult(scl);
}

function keyPressed() {
    if (keyCode === UP_ARROW){
        player.changeDirection(Direction.NORTH);
    } else if (keyCode === DOWN_ARROW) {
        player.changeDirection(Direction.SOUTH);
    } else if (keyCode === RIGHT_ARROW) {
        player.changeDirection(Direction.EAST);
    } else if (keyCode === LEFT_ARROW) {
        player.changeDirection(Direction.WEST);
    }
}

function mousePressed() {
    player.total++;
}


function Snake() {
    this.x = 0;
    this.y = 0;
    this.xspeed = 1;
    this.yspeed = 0;
    this.total = 0;
    this.tail = [];
    this.currentDir = Direction.EAST

    this.death = function() {
        console.log("running death");
        for(var i; i < this.total; i++){
            var pos = this.tail[i];
            var d = dist(this.x, this.y, pos.x, pos.y);
            console.log(pos);
            if(d < 1){
                console.log("hit yourself fool");
                this.total = 0;
                this.tail = [];
            }
        }
    }

    this.eat = function(pos) {
        var d = dist(this.x, this.y, pos.x, pos.y);

        if(d < 1) {
            this.total++;
            return true;
        } else {
            return false;
        }
    }

    this.update = function(){
        this.death();
        if (this.tail.length === this.total) {
            for (var x = 0; x < this.total-1; x++) {
                this.tail[x] = this.tail[x+1];
            }
            this.tail[this.total-1] = createVector(this.x,this.y);
        } else {
            this.tail[this.total-1] = createVector(this.x,this.y);
        }

        this.x += this.xspeed * scl;
        this.y += this.yspeed * scl;

        this.x = constrain(this.x, 0, width-scl);
        this.y = constrain(this.y, 0, height-scl);
    }

    this.changeDirection = function(d){
        prop = createVector(d.x - this.currentDir.x, d.y - this.currentDir.y);
        if (prop.x == 0 | prop.y == 0) {
            // do nothing, this is  a zero'ed vecotr
        } else {
            this.xspeed = d.x;
            this.yspeed = d.y;
            this.currentDir = d;
        }
    }

    this.show = function() {
        // draw tail
        fill(255)
        for(var i = 0; i < this.total; i++) {
            rect(this.tail[i].x, this.tail[i].y, scl, scl);
        } 

        // print head
        rect(this.x, this.y, scl, scl)
    }
}
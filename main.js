// example

const { AStar } = await import("./astar.js");

const X = 9;
const Y = 9;

function wrapAStar(arr) {
    let input_str = "";
    for (let i = 0; i < X; i++) {
        for (let j = 0; j < Y; j++) {
            input_str += arr[i][j];
        }
    }
    const csv = AStar.Find(input_str);
    const out = csv.split("\n").map(x => x.split(",").map(y => Number(y)));
    return out;
}
export interface Coordinate {
    text?: string;
    latitude:number;
    longitude:number;
}
// a coordinate can be decomposed into latitude and longitude, 
// which allows us to express in a mathematical exact manner
// any single point on Earth.
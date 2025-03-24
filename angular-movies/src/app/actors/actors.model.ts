export interface ActorDTO{
    id: number;
    name: string;
    dateOfBirth:Date;
    picture?:string;
}

export interface ActorCreationDTO{
    name:string;
    dateOfBirth:Date;
    picture?:File;
}
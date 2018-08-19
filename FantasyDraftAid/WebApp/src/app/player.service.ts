import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Player} from './player';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  constructor(private httpClient: HttpClient) {
  }

  public async getPlayers(): Promise<Player[]> {
    const baseUri = 'http://localhost:5000/players?position=';
    const promises: Promise<Player[]>[] = [];
    promises.push(this.httpClient.get<Player[]>(`${baseUri}qb`).toPromise());
    promises.push(this.httpClient.get<Player[]>(`${baseUri}wr`).toPromise());
    promises.push(this.httpClient.get<Player[]>(`${baseUri}te`).toPromise());
    promises.push(this.httpClient.get<Player[]>(`${baseUri}rb`).toPromise());
    promises.push(this.httpClient.get<Player[]>(`${baseUri}k`).toPromise());

    const players = await Promise.all(promises);
    return Promise.resolve([].concat(...players));
  }
}

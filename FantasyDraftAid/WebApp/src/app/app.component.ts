import {Component, OnInit, ViewChild} from '@angular/core';
import {Player} from './player';
import {PlayerService} from './player.service';
import {AgGridNg2} from 'ag-grid-angular';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  @ViewChild('agGrid') agGrid: AgGridNg2;
  players: Promise<Player[]>;

  columnDefs = [
    {headerName: 'Full Name', field: 'fullName', checkboxSelection: true},
    {headerName: 'First', field: 'firstName'},
    {headerName: 'Last', field: 'lastName'},
    {headerName: 'Team', field: 'teamName'},
    {headerName: 'Years Pro', field: 'yearsPro'},
    {headerName: 'Pos', field: 'position'},
    {headerName: 'Url', field: 'profileUrl'},
    {headerName: 'Status', field: 'playerStatus'},
    {headerName: 'GPPS', field: 'averageGamesPlayedPerSeason'},
    {headerName: 'LYP', field: 'priorYearPoints'},
    {headerName: 'LYPPG', field: 'priorYearPointsPPG'},
    {headerName: 'LYGP', field: 'priorYearGamesPlayed'},
    {headerName: '2YP', field: 'twoYearPoints'},
    {headerName: '2YPPG', field: 'twoYearPointsPPG'},
    {headerName: '2YGP', field: 'twoYearGamesPlayed'},
    {headerName: '3YP', field: 'threeYearPoints'},
    {headerName: '3YPPG', field: 'threeYearPointsPPG'},
    {headerName: '3YGP', field: 'threeYearGamesPlayed'},
    {headerName: '3 Year Avg', field: 'threeYearAverage'}
  ];

  constructor(private playerService: PlayerService) {
  }

  ngOnInit(): void {
    this.players = this.playerService.getPlayers();
  }

  removeSelected() {
    this.agGrid.api.updateRowData({
      remove: this.agGrid.api.getSelectedNodes().map(node => node.data)
    });
  }

  showAll() {
    this.agGrid.api.setQuickFilter('');
  }

  showQuarterbacks() {
    this.agGrid.api.setQuickFilter('Quarterback');
  }


  showRunningBacks() {
    this.agGrid.api.setQuickFilter('Running Back');
  }

  showWideReceivers() {
    this.agGrid.api.setQuickFilter('Wide Receiver');
  }

  showTightEnds() {
    this.agGrid.api.setQuickFilter('Tight End');
  }

  showKickers() {
    this.agGrid.api.setQuickFilter('Kicker');
  }

  onGridReady(params) {
    params.api.sizeColumnsToFit();
  }

  quickSearch(input: string) {
    this.agGrid.api.setQuickFilter(input);
  }
}

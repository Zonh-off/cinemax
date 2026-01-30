import {Component, Input, inject, signal, OnInit} from '@angular/core';
import { IonicModule, ModalController } from '@ionic/angular';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-genre-filter',
  standalone: true,
  imports: [IonicModule, CommonModule],
  template: `
    <ion-header class="ion-no-border">
      <ion-toolbar class="ion-no-margin ion-no-padding">
        <ion-buttons class="flex flex-row justify-between">
          <ion-button (click)="reset()" color="danger">Reset</ion-button>
          <ion-button (click)="confirm()" weight="bold">Done</ion-button>
        </ion-buttons>
      </ion-toolbar>
    </ion-header>

    <ion-content class="ion-padding">
      <div class="flex flex-wrap justify-center gap-2">
        @for (genre of allGenres; track genre) {
          <ion-chip
            [color]="tempSelected().includes(genre) ? 'primary' : 'medium'"
            [outline]="!tempSelected().includes(genre)"
            (click)="toggleGenre(genre)"
            class="m-0 py-3 px-2 text-base">
            <ion-label>{{ genre }}</ion-label>
          </ion-chip>
        }
      </div>
    </ion-content>
  `
})
export class GenreFilterComponent implements OnInit {
  private modalCtrl = inject(ModalController);

  @Input() allGenres: string[] = [];
  @Input() initialSelected: string[] = [];

  tempSelected = signal<string[]>([]);

  ngOnInit() {
    this.tempSelected.set([...this.initialSelected]);
  }

  toggleGenre(genre: string) {
    this.tempSelected.update(current =>
      current.includes(genre) ? current.filter(g => g !== genre) : [...current, genre]
    );
  }

  reset() {
    this.tempSelected.set([]);
  }

  confirm() {
    this.modalCtrl.dismiss(this.tempSelected());
  }
}

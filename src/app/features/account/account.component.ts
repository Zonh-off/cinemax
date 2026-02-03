import {Component, inject} from '@angular/core';
import {AccountService} from '../../core/services/account.service';
import {IonicModule} from '@ionic/angular';
import {Router} from '@angular/router';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css'],
  imports: [
    IonicModule
  ],
  standalone: true
})
export class AccountComponent {
  accountService = inject(AccountService);
  router = inject(Router);

  onLogout() {
    this.accountService.logout().subscribe({
      next: () => {
        this.accountService.currentUser.set(null);
        this.router.navigateByUrl('/');
      }
    });
  }
}

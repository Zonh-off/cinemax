import {Component, inject} from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {IonicModule} from '@ionic/angular';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {AppInputComponent} from '../../../../shared/components/text-input/text-input.component';
import {AccountService} from '../../../../core/services/account.service';
import {switchMap} from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [
    IonicModule,
    FormsModule,
    RouterLink,
    ReactiveFormsModule,
    AppInputComponent
  ],
  standalone: true
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private accountService = inject(AccountService);

  loginForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]]
  });

  onLogin() {
    if (this.loginForm.valid) {
      this.accountService.login(this.loginForm.value).pipe(
        switchMap(() => this.accountService.getUserInfo())
      ).subscribe({
        next: () => this.router.navigateByUrl('/tabs/home')
      });
    }
  }
}

import {Component, inject, OnInit, signal} from '@angular/core';
import {IonicModule} from '@ionic/angular';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {AccountService} from '../../../../core/services/account.service';
import {AppInputComponent} from '../../../../shared/components/text-input/text-input.component';

@Component({
  selector: 'app-register',
  imports: [
    IonicModule,
    FormsModule,
    AppInputComponent,
    ReactiveFormsModule
  ],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: true
})
export class RegisterComponent implements OnInit {
  private fb = inject(FormBuilder)
  private router = inject(Router)
  private accountService = inject(AccountService);

  step = signal(1);
  codeError = signal<string | null>(null);
  isVerifying = signal(false);

  registerForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    repeatPassword: ['', [Validators.required]],
  });

  verificationForm: FormGroup = this.fb.group({
    code: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
  });

  ngOnInit() {
    this.verificationForm.get('code')?.valueChanges.subscribe(() => {
      this.codeError.set(null);
    });
  }

  get passwordsMatch() {
    const p = this.registerForm.get('password')?.value;
    const r = this.registerForm.get('repeatPassword')?.value;
    return p === r;
  }

  onNextStep() {
    if (this.registerForm.valid && this.passwordsMatch) {
      const e = this.registerForm.get('email')?.value;
      const p = this.registerForm.get('password')?.value;
      this.accountService.preRegister({
        email: e,
        password: p
      }).subscribe();
      this.step.set(2);
    }
  }

  onVerify() {
    if (this.verificationForm.valid) {
      const code = this.verificationForm.get('code')?.value;
      const email = this.registerForm.get('email')?.value;

      this.codeError.set(null);
      this.isVerifying.set(true);

      this.accountService.verifyEmail(email, code).subscribe({
        next: () => {
          this.isVerifying.set(false);
          this.router.navigateByUrl('/auth/login');
        },
        error: (err) => {
          this.isVerifying.set(false);

          if (err.status === 400 || err.status === 401) {
            this.codeError.set('Incorrect or expired code. Please try again.');
          } else {
            this.codeError.set('Something went wrong. Please try later.');
          }
        }
      });
    }
  }
}

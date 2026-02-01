import { Component, input, Self, Optional } from '@angular/core';
import { ReactiveFormsModule, FormControl, ControlValueAccessor, NgControl } from '@angular/forms';
import { IonLabel, IonInput, IonText, IonInputPasswordToggle } from '@ionic/angular/standalone';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [ReactiveFormsModule, IonLabel, IonInput, IonText, IonInputPasswordToggle],
  template: `
    <div class="flex flex-col items-start w-full mb-4">
      @if (label()) {
        <ion-label class="mb-2 font-semibold text-sm">{{ label() }}</ion-label>
      }

      <ion-input
        [formControl]="control"
        [type]="type()"
        [placeholder]="placeholder() || label()"
        [class.border-red-500]="isInvalid()"
        class="bg-gray-200 font-medium rounded-lg border-2 border-transparent transition-all w-full h-14"
        style="--padding-start: 16px; --padding-end: 16px"
      >
        @if (type() === 'password') {
          <ion-input-password-toggle slot="end"></ion-input-password-toggle>
        }
      </ion-input>

      @if (isInvalid()) {
        <ion-text color="danger" class="text-xs mt-1 ml-1">
          {{ errorMessage() }}
        </ion-text>
      }
    </div>
  `,
  styles: `
    ion-input.border-red-500 {
      --background: #fff5f5;
    }
    ion-input {
      --padding-bottom: 0;
      --padding-top: 0;
    }
  `
})
export class AppInputComponent implements ControlValueAccessor {
  label = input<string>('');
  placeholder = input<string>('');
  type = input<'text' | 'email' | 'password' | 'number'>('text');
  errorText = input<string>('This field is invalid');

  constructor(@Self() @Optional() public controlDir: NgControl) {
    if (this.controlDir) {
      this.controlDir.valueAccessor = this;
    }
  }

  get control(): FormControl {
    return (this.controlDir?.control as FormControl) || new FormControl();
  }

  isInvalid(): boolean {
    return (this.control.invalid&&(this.control.dirty||this.control.touched));
  }

  errorMessage(): string {
    const errors = this.control.errors;
    if (errors?.['required']) return 'This field is required';
    if (errors?.['email']) return 'Please enter a valid email';
    if (errors?.['minlength']) return `Minimum ${errors['minlength'].requiredLength} characters`;
    return this.errorText();
  }

  writeValue(obj: any): void {}
  registerOnChange(fn: any): void {}
  registerOnTouched(fn: any): void {}
  setDisabledState(isDisabled: boolean): void {}
}

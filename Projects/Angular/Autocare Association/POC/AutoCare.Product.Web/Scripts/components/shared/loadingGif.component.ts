import { Component, Input } from '@angular/core'

@Component({
    selector: 'loading-gif',
    templateUrl: 'app/templates/shared/loadingGif.component.html'
})

export class LoadingGifComponent {
    @Input('show') show: boolean = false;
}
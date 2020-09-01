import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'footer-comp',
    templateUrl: 'app/templates/shared/footer.component.html'
})
export class FooterComponent implements OnInit{
    year: number;

    ngOnInit() {
        this.year = new Date().getFullYear();
    }
}
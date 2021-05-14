import { NO_ERRORS_SCHEMA } from "@angular/core";
import { ErrorComponent } from "./error.component";
import { ComponentFixture, TestBed } from "@angular/core/testing";

describe("ErrorComponent", () => {

  let fixture: ComponentFixture<ErrorComponent>;
  let component: ErrorComponent;
  beforeEach(() => {
    TestBed.configureTestingModule({
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
      ],
      declarations: [ErrorComponent]
    });

    fixture = TestBed.createComponent(ErrorComponent);
    component = fixture.componentInstance;

  });

  it("should be able to create component instance", () => {
    expect(component).toBeDefined();
  });
  
});

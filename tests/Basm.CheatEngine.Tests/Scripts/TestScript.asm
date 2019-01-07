[ENABLE]
label(patchAddress)
registersymbol(patchAddress)

"BasmTest.exe"+0001384: // we are replacing seven bytes
//CodeRegion:
// movzx   eax, cs:?isVulnerable@@3_NA ; bool isVulnerable original
// depending on the value of isVulnerable, the program executes a different set of instructions 
// we want to avoid the set where isVulnerable is true as that is the 'bad' code
patchAddress: // set patch address to be defined
mov eax, 0 // patch, 5 bytes
nop // 1 byte
nop // 1 byte

"BasmTest.exe"+0001463: // we are replacing three bytes
//  movzx   eax, al
// if eax is 1, then the program will set shouldCrash=1, which when checked in 'badFunction' will crash the program
// here we set it to always be 0, so shouldCrash is never set
mov al, 0 // patch, 2 bytes
nop // 1 byte

"BasmTest.exe"+0001512: // we are replacing seven bytes
// set rcx to NULL for the pathname to stop any process creation
mov rcx, 0 // patch, 7 bytes

[DISABLE]
unregistersymbol(patchAddress)
